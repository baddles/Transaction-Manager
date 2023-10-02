import { Component } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { delay, timer } from 'rxjs';
import * as CryptoJS from 'crypto-js';
import { ImgurService } from '../imgur.service';
@Component({
  selector: 'app-callback',
  templateUrl: '../imgur.component.html',
  styleUrls: ['../oauth.component.scss']
})
export class CallbackComponent {
  
  status = 'Checking imgur credentials...';
  public matched = true;
  public finished = false;
  constructor(
    private route: ActivatedRoute,
    private cookieService: CookieService,
    private imgurService: ImgurService,
    private router: Router
  ) {}
    ngOnInit() {
      this.route.queryParams.pipe(
        delay(300)
      ).subscribe((params: any) => {
        if (!(params.state && params.code)) {
          this.imgurResponseError(params)
        }
        else if (params.state !== this.cookieService.get("state")) {
          this.imgurResponseError('State not match! Possible CSRF!')
        }
        else {
          sessionStorage.setItem("code", params.code);
          this.finished = true;
          this.status = "Redirecting to login..."
          timer(1250).subscribe(() => {
          this.router.navigateByUrl("../login")
          })
          /*
          this.imgurService.exchangeToken(params.code).subscribe((response: any) => {
            if (!("access_token" in response && "token_type" in response)) {
              this.imgurResponseError(response)
            }
            else {
              localStorage.setItem("access_token", response.access_token);
              localStorage.setItem("token_type", response.token_type);
              localStorage.setItem("scope", response.scope)

            }
          },
          (error) => {
            this.imgurResponseError(error)
          })
          */
        }
      })
    }
    private imgurResponseError(response: any | string) {
      this.matched = false
      if (typeof(response) === 'string') {
        this.status = response;
      }
      else {
        if ("error_description" in response) {
          console.error(response.error + "\n" + response.error_description + "\n" + response.error_uri)
          this.status = response.error_description
        }
        else {
          this.status = "Server returned " + response.status.toString()
        }
      }
    }
}
