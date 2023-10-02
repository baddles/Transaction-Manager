import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) {
  }
  login(username: string, encryptedPassword: string, code: string, salt: string) {
    return this.http.post('/api/auth/login', {
      username,
      encryptedPassword,
      code,
      salt
    })
  }
  getOAuthToken() {
    return this.http.get('https://github.com/login/oauth/authorize', {
      headers: {
      },
      params: {
        'client_id': '0c6a4987ca4f59d7626d',
        'login': 'baddles',
        'scope': 'repo',
        'redirect_uri': 'https://localhost:4200/callback',
        'repo': 'baddles/transaction_manager_db',
        'allow_signup': 'false',
        'state': '1477303702'
      }
    })
  }
}
