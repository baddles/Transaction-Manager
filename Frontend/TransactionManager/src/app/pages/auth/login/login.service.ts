import { Injectable } from '@angular/core';
import API_PATH from '../../../../assets/API/API_PATH.json'
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  constructor(private http: HttpClient) { }
  postLogin(username: string, encryptedPassword: string, salt: string) {
    return this.http.post(API_PATH.api.auth.Login, {
      username,
      encryptedPassword,
      salt
    })
  }
}
