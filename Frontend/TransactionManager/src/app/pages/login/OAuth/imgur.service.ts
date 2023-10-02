import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import API_KEY from '../../../../assets/API/API_KEY.json'
import CONFIG from '../../../../assets/configs/config.json'
import API_PATH from '../../../../assets/API/API_PATH.json'
@Injectable({
  providedIn: 'root'
})
export class ImgurService {
  constructor(private httpClient: HttpClient) {
    
  }
  getInfo(state: string) {
    return this.httpClient.get("/api/auth/initialize", {
      headers: {
        state
      }
    })
  }
  authorize(state: string): string {
    return API_PATH['imgur.com'].login.authorize
    + "?client_id=" + sessionStorage.getItem("client_id")
    + '&response_type=' + CONFIG['imgur.com'].authorize.response_type
    + '&state=' + state;
  }
  authenticate(username: string, hashedPassword: string, code: string) {
    const salt = sessionStorage.getItem("state");
  }
  exchangeToken(code: string) {
    return this.httpClient.post("/getAccessToken", {code},
    {
      headers: {
        "content-type": "application/json"
      }
    }
    )
  }
}
