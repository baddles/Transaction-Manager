import { Component, OnInit } from '@angular/core';
import { RngService } from './rng.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { initializeResponseData } from './auth.response.model';
import { catchError, map } from 'rxjs';
import { HttpStatusCode } from '@angular/common/http';
import { GenericResponseDTO } from 'src/assets/common/GenericResponseDTO';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss', './spinner.component.scss']
})
export class AuthComponent implements OnInit {
  status: string;
  matched: boolean;
  finished: boolean;
  constructor(public randomService: RngService, public cookie: CookieService, private route: Router, public authenticationService: AuthService) {
    this.status = "Initializing...";
    this.matched = true;
    this.finished = false;
    sessionStorage.setItem("state", randomService.getSeed());
  }
  ngOnInit(): void {
    this.cookie.set("state", this.randomService.invoke().toString(), 1/24/60*10);
    this.authenticationService.getInfo(sessionStorage.getItem("state") || this.randomService.getSeed()).pipe(catchError((error) => {
      this.matched = false;
      if (typeof(error) === "string") {
        this.status = error;
      }
      return error;
    }), map((response: any) => {
      if (typeof(response) !== "string") {
        if (!response) {
          this.status = "Invalid response data!"
        }
        if (response.httpStatusCode !== 200) {
          this.status = response.message || response.httpStatusCode
        }
        if (!(response.data.client_id && response.data.public_key)) {
          response.HttpStatusCode = HttpStatusCode.NoContent
          this.status = "Bad response data!"
        }
      }
      return response;
    })).subscribe((response: GenericResponseDTO<initializeResponseData>) => {
      if (response.httpStatusCode === 200) {
        sessionStorage.setItem("client_id", response.data.client_id);
        sessionStorage.setItem("public_key", response.data.public_key);
        this.route.navigateByUrl("imgur");
      }
    })
  }
}
