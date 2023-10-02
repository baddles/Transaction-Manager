import { Component, ElementRef, ViewChild } from '@angular/core';
import { Observable, interval, map, timer } from 'rxjs';
import { LoginService } from './login.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import * as CryptoJS from 'crypto-js';
import * as JsEncryptModule from 'jsencrypt';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  username: string = ""
  password: string = ""
  status: string = ""
  @ViewChild('centerbox', {static: false}) centerBox: ElementRef | undefined;
  @ViewChild('title', {static: false}) title: ElementRef | undefined;
  encryptMod;
  constructor(private loginService: LoginService,
              private cookieService: CookieService,
              private router: Router) {
    this.encryptMod = new JsEncryptModule.JSEncrypt();
  }
  ngOnInit() {
    if (sessionStorage.getItem("code") == null || sessionStorage.getItem("public_key") == null) {
      this.router.navigate(["/"])
    }
  }
  login() {
    let hashedPassword = CryptoJS.SHA512(CryptoJS.SHA256(this.password + CryptoJS.MD5(this.password))).toString()
    var code = sessionStorage.getItem("code");
    var public_key = sessionStorage.getItem("public_key");
    if (code === null || public_key === null) {
      this.status = 'Cannot get storage data. Please try again!'
      return this.failed();
    }
    else {
      this.encryptMod.setPublicKey(sessionStorage.getItem("public_key") || "");
      let encryptedData = this.encryptMod.encrypt(hashedPassword).toString();
      this.loginService.login(this.username, encryptedData || "", code || "", sessionStorage.getItem("state") || "").subscribe((response: any | string | object) => {
        if (response.message !== "Success") {
          this.status = response.message;
          this.failed() 
        }
        else {
          // set two containers, one is Authorization, one is OAuthToken
          /**
           * SessionStorage.Authorization: {
           *    access_token: "Bearer + " ${access_token}
           *    expiration_time: ...
           * }
           * SessionStorage.OAuth: {
           *    access_token: "Bearer + " ${access_token}
           *    expiration_time: ...
           * }
           * LocalStorage.OAuth : {
           *    refresh_token: ${imgurOAuth2Token.refresh_token}
           * }
           */
          sessionStorage.setItem("imgur_access_token", response.data.imgurOAuth2Token.access_token);
          sessionStorage.setItem("tm_access_token", response.data.tmToken.access_token);
          localStorage.setItem("imgur_refresh_token", response.data.imgurOAuth2Token.refresh_token);
          localStorage.setItem("tm_refresh_token", response.data.tmToken.refresh_token);
          localStorage.setItem("imgur_expiration_time", response.data.imgurOAuth2Token.expiration_time);
          localStorage.setItem("tm_expiration_time", response.data.tmToken.expiration_time);
        }
      }, (error) => {
        if (this.centerBox) {
          this.centerBox.nativeElement.style.minHeight = '360px';
          this.centerBox.nativeElement.style.marginTop = '40px';
        }
        if (this.title) {
          this.title.nativeElement.style.marginTop = '-40px';
        }
        if (error.status === 401) {
          this.status = error.error;
        }
        else {
          this.status = error.response.message || error.error || "Internal Server Error, please try again.";
          this.failed();
        }
      })
    }
  }
  failed() {
    this.cookieService.deleteAll()
    sessionStorage.clear()
    localStorage.clear()
    timer(1250).subscribe(() => {
     // this.router.navigate(["/"])
    })
  }


}
