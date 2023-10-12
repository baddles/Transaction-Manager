import { Component } from '@angular/core';
import { ImgurService } from './imgur.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-imgur',
  templateUrl: '../auth.component.html',
  styleUrls: ['../auth.component.scss','../spinner.component.scss']
})
export class ImgurComponent {
  state: string;
  status: string;
  matched: boolean;
  finished: boolean;
  constructor(private imgurService: ImgurService, private cookie: CookieService) {
    this.status = "Redirecting to Imgur...";
    this.matched = true;
    this.finished = false;
    this.state = this.cookie.get("state");
  }
  ngAfterViewInit() {
    window.location.href = this.imgurService.authorize(this.state)
  }
}
