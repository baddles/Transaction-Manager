import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

import API_KEY from '../../../assets/API/API_KEY.json'
import API_PATH from '../../../assets/API/API_PATH.json'
import CONFIG from '../../../assets/configs/config.json'
import { catchError, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RngService {
  useRandomORGGeneration: boolean;
  seed: string;
  constructor(private http: HttpClient, private cookieService: CookieService) {
    this.useRandomORGGeneration = CONFIG["random.org"].enable;
    function generateRandom64BitNumber(): string {
      const array = new Uint32Array(2);
      crypto.getRandomValues(array);
      const random64BitNumber = (BigInt(array[0]) << 32n) + BigInt(array[1]);
      return random64BitNumber.toString();
    }
    this.seed = generateRandom64BitNumber();
  }
  public getSeed(): string {
    return this.seed;
  }
  invoke() {
    if (this.useRandomORGGeneration) {
      return this.externalGenerate(this.seed).pipe(
        catchError((error) => {
          console.error(error);
          console.warn("Falling back to internal generation.");
          return this.internalGenerate(this.seed);
        }),
        map((response: any) => {
          if ('error' in response) {
            console.error("RANDOM.ORG API returned error " + response.error.code + ': ' + response.error.message);
            console.warn("Falling back to internal generation.");
            return this.internalGenerate(this.seed);
          } else {
            return response.result.random.data[0] || "";
          }
        })
      );
    } else {
      return this.internalGenerate(this.seed);
    }
  }
  private internalGenerate(id: string): string {
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
    return generateRandomString(BigInt(id || Math.floor((new Date()).getTime() /1000)),32,CONFIG['random.org'].characters)
  }
  private externalGenerate(id: string) {
    return this.http.post(API_PATH['random.org'].invoke, {
      'jsonrpc': CONFIG['random.org'].jsonrpg,
      'method': CONFIG['random.org'].method.SignedAPI.generateSignedStrings,
      'params': {
        'apiKey': API_KEY['random.org'].apiKey,
        'n': CONFIG['random.org'].n,
        'length': CONFIG['random.org'].length,
        'characters': CONFIG['random.org'].characters
      },
      'id': id
    })
  }
}
