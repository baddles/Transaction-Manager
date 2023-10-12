import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Observable, catchError, config, interval, map, timer } from 'rxjs';
import { LoginService } from './login.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import * as CryptoJS from 'crypto-js';
import * as JsEncryptModule from 'jsencrypt';
import { LoginContext } from './login.context';
import { GenericResponseDTO } from 'src/assets/common/GenericResponseDTO';
import { loginResponseData } from '../auth.response.model';
import { LoginFailedState, LoginInitialState, LoginLoadingState, LoginSuccessState } from './login.state';
import { HttpStatusCode } from '@angular/common/http';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss', '../spinner.component.scss']
})
export class LoginComponent implements OnInit, AfterViewInit {
  username: string 
  password: string
  status: string
  publicKey: string | null;
  state: string | null;
  currentContext: string;
  @ViewChild('centerbox', {static: false}) centerBox: ElementRef | undefined;
  @ViewChild('title', {static: false}) title: ElementRef | undefined;
  encryptMod;
  public context: LoginContext
  constructor(private loginService: LoginService,
    private cookieService: CookieService,
    private router: Router) {
    this.username = sessionStorage.getItem("account_username") || ""
    this.password = ""
    this.status = ""
    this.encryptMod = new JsEncryptModule.JSEncrypt();
    this.publicKey = sessionStorage.getItem("public_key");
    this.state = sessionStorage.getItem("state")
    this.context = new LoginContext();
    this.currentContext = this.context.request();
  }
  ngOnInit(): void {
    if (this.publicKey === null || this.state === null) {
      return this.failed("Cannot get storage data. Re-initializing flow.")
    }
  }
  ngAfterViewInit(): void {
    if (this.currentContext === "Failed") {
      this.unsuccessful();
    }
  }
  private initial() {
    if (this.centerBox) {
      this.centerBox.nativeElement.style.minHeight = '320px';
      this.centerBox.nativeElement.style.marginTop = '0'
    }
    if (this.title) {
      this.title.nativeElement.style.marginTop = '0';
    }
    this.status = "";
  }
  private processing() {
    if (this.centerBox) {
      this.centerBox.nativeElement.style.minHeight = '360px'; // 320px
      this.centerBox.nativeElement.style.marginTop = '40px'; // 0
    }
    if (this.title) {
      this.title.nativeElement.style.marginTop = '-40px'; // 0
    }
  }
  private unsuccessful() {
    if (this.centerBox) {
      this.centerBox.nativeElement.style.minHeight = '400px'; // 320px
      this.centerBox.nativeElement.style.marginTop = '80px'; // 0
    }
    if (this.title) {
      this.title.nativeElement.style.marginTop = '-80px'; // 0
    }
  }
  private access_token: string = ""; // For the
  private token_type: string = ""; // compiler
  private refresh_token: string = ""; // to not yell
  private expires_at: string = ""; // missing vars.
  private successfully() {
    sessionStorage.setItem("Authorization", this.token_type + " " + this.access_token);
    localStorage.setItem("RefreshToken", this.refresh_token);
    localStorage.setItem("Expiration", this.expires_at);
    sessionStorage.removeItem("public_key");
    sessionStorage.removeItem("state");
  }
  login() {
    this.context.setState(new LoginLoadingState(this, this.processing));
    this.currentContext = this.context.request()
    let encryptedPassword = this.encryptPassword();
    if (encryptedPassword === false) {
      return this.failed("Cannot encrypt password, please check Public Key! Re-initializing flow.")
    }
    this.loginService.postLogin(this.username, encryptedPassword, this.state || "").pipe(
      catchError((error) => {
        if (error.status !== 401) {
          console.error(error);
          this.failed(error.error.message || error.statusText);
        }
        else {
          this.failed(error.error.message, false);
        }
        return error.error.message || error.statusText || "Internal Server Error";
      }),
      map((response: any) => {
        console.log(response);
        if (!response.data || response.data === null || Object.keys(response.data).length === 0) {
          response.httpStatusCode = HttpStatusCode.NoContent
          this.failed("204 - Response data is empty");
        }
        if (!(response.data.access_token && response.data.token_type && response.data.refresh_token && response.data.expires_at)) {
          response.httpStatusCode = HttpStatusCode.NonAuthoritativeInformation
          this.failed("203 - Invalid response data");
        }
        return response;
      })
    ).subscribe((response: GenericResponseDTO<loginResponseData>) => {
      if (response.httpStatusCode === 200) {
        this.success(response.data as loginResponseData);
      }
    })
  }
  encryptPassword(): string | false {
    let hashedPassword = CryptoJS.SHA512(CryptoJS.SHA256(this.password + CryptoJS.MD5(this.password))).toString();
    this.encryptMod.setPublicKey(this.publicKey || "");
    return this.encryptMod.encrypt(hashedPassword);
  }
  failed(message: string, redirect: boolean = true) {
    this.status = message;
    this.context.setState(new LoginFailedState(this, this.unsuccessful));
    this.currentContext = this.context.request()
    this.cookieService.deleteAll()
    sessionStorage.clear()
    localStorage.clear()
    if (redirect === true) {
      timer(1250).subscribe(() => {
        this.router.navigate(["/"]);
      })
    }
  }
  success(data: loginResponseData) {
    this.context.setState(new LoginSuccessState(data, this.successfully));
    this.currentContext = this.context.request()
    timer(1250).subscribe(() => {
      this.router.navigate(["/dashboard"]);
    })
  }
  onInfoChange() {
    if (this.currentContext !== "") {
      this.context.setState(new LoginInitialState(this, this.initial));
      this.currentContext = this.context.request()
    }
  }
}
