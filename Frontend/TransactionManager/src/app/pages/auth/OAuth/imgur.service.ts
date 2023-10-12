import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import CONFIG from '../../../../assets/configs/config.json'
import API_PATH from '../../../../assets/API/API_PATH.json'

@Injectable({
  providedIn: 'root'
})
export class ImgurService {

  constructor() { }
  authorize(state: string): string {
    return API_PATH['imgur.com'].login.authorize
    + "?client_id=" + sessionStorage.getItem("client_id")
    + '&response_type=' + CONFIG['imgur.com'].authorize.response_type
    + '&state=' + state;
  }
}
