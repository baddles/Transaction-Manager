import { Component } from '@angular/core';
import { RandomService } from '../random.service';
import { CookieService } from 'ngx-cookie-service'
import { GithubService } from './github.service';
import CONFIG from '../../../../../assets/configs/config.json'
@Component({
  selector: 'app-github',
  templateUrl: './github.component.html',
  styleUrls: ['../oauth.component.scss']
})
export class GithubComponent {
  state
  constructor(private randomService: RandomService, private cookieService: CookieService, private githubService: GithubService) {
    function generateRandom64BitNumber(): string {
      const array = new Uint32Array(2);
      crypto.getRandomValues(array);
      const random64BitNumber = (BigInt(array[0]) << 32n) + BigInt(array[1]);
      return random64BitNumber.toString();
    }
    this.state = generateRandom64BitNumber();
  }
  ngOnInit() {
    if (this.testToken()) {

    }
    this.githubService.getInfo(this.state.toString()).subscribe((response: any) => {
      if (sessionStorage.getItem("client_id")) {
        sessionStorage.removeItem("client_id")
      }
      if (sessionStorage.getItem("redirect_uri")) {
        sessionStorage.removeItem("redirect_uri")
      }
      sessionStorage.setItem("client_id", response.client_id.toString())
      sessionStorage.setItem("redirect_uri", response.redirect_uri.toString())
      sessionStorage.setItem("state", this.state)
      sessionStorage.setItem("public_key", response.public_key);
      if (CONFIG['random.org'].enable) {
        this.randomService.invoke(this.state).subscribe((response: any) => {
          if ('error' in response) {
            console.error("RANDOM.ORG API returned error " + response.error.code + ': ' + response.error.message)
          }
          else {
            console.log(response)
            this.state = response.result.random.data[0]
          }
          this.callGitHub()
        },
        (error) => {
          this.callGitHub()
        })
      }
      else {
        function generateRandomString(seed: bigint, length: number, alphabet: string) {
          let result = '';
          const alphabetLength = alphabet.length;
          
          // Ensure the seed is a non-negative BigInt
          seed = seed < 0 ? -seed : seed;
        
          for (let i = 0; i < length; i++) {
            // Use the least significant 6 bits of the seed to index the alphabet
            const index = Number(seed & 0x3Fn);
            result += alphabet.charAt(index);
            
            // Right shift the seed by 6 bits for the next character
            seed >>= 6n;
          }
        
          return result;
        }
        this.state = generateRandomString(BigInt(sessionStorage.getItem("state") || Math.floor((new Date()).getTime() /1000)),32,CONFIG['random.org'].characters)
      }
      this.callGitHub()
    })
  }
  testToken() {
    if (localStorage.getItem("access_token") === null || localStorage.getItem("token_type") === null || localStorage.getItem("scope") === null) {
      return false;
    }
    else {
      return true;
    }
  }
  callGitHub() {
    this.cookieService.set("state", this.state, 1/24/60*10)
    window.location.href = this.githubService.authorize(this.state)
  }
}
