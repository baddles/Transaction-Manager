import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import API_KEY from '../../../../../assets/API/API_KEY.json'
import CONFIG from '../../../../../assets/configs/config.json'
import API_PATH from '../../../../../assets/API/API_PATH.json'
@Injectable({
  providedIn: 'root'
})
export class GithubService {

  constructor(private httpClient: HttpClient) {
    
  }
  getInfo(state: string) {
    return this.httpClient.get("/api/GitHubOauth/info", {
      headers: {
        state
      }
    })
  }
  authorize(state: string): string {
    return API_PATH['github.com'].login.authorize
    + "?client_id=" + sessionStorage.getItem("client_id")
    + '&redirect_uri=' + sessionStorage.getItem("redirect_uri")
    + '&login=' + CONFIG['github.com'].authorize.login
    + '&scope=' + CONFIG['github.com'].authorize.scope
    + '&repo=' + CONFIG['github.com'].authorize.repo
    + '&allow_signup=' + CONFIG['github.com'].authorize.allow_signup
    + '&state=' + state;
  }
  authenticate(username: string, hashedPassword: string, code: string) {
    const salt = sessionStorage.getItem("state");
  }
  exchangeToken(code: string) {
    return this.httpClient.post("/api/GitHubOauth/getAccessToken", {code},
    {
      headers: {
        "content-type": "application/json"
      }
    }
    )
  }
}
