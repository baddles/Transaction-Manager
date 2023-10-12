import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import API_PATH from '../../../assets/API/API_PATH.json'
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private httpClient: HttpClient) { }
  getInfo(state: string): Observable<Object> {
    return this.httpClient.get(API_PATH.api.auth.Init, {
      headers: {
        state
      }
    })
  }
  postRefresh(username: string, refresh_token: string) {
    return this.httpClient.post(API_PATH.api.auth.Refresh, {
      username,
      refresh_token
    })
  }
}

