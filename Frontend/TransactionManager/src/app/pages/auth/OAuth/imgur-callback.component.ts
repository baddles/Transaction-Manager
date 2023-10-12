import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { delay, take } from 'rxjs';
import CONFIG from '../../../../assets/configs/config.json'
@Component({
  selector: 'app-imgur-callback',
  templateUrl: '../auth.component.html',
  styleUrls: ['../auth.component.scss', '../spinner.component.scss']
})
export class ImgurCallbackComponent {
  status: string;
  matched: boolean;
  finished: boolean;
  constructor(
    private route: ActivatedRoute,
    private cookie: CookieService,
    private router: Router
  ) {
    this.status = "Checking imgur credentials..."
    this.matched = true;
    this.finished = false;
  }
  ngOnInit() {
    this.route.queryParams.pipe(delay(300), take(1)).subscribe((params: any) => {
      if (!params || params.error || !params.state) {
        this.imgurResponseError("No request body returned from Imgur!" || params.error || 'No state parameter found');
        return;
      }

      const state = params.state;
      if (state !== this.cookie.get('state')) {
        this.imgurResponseError('State does not match! Possible CSRF!');
        return;
      }

      this.handleFragment();
    });
  }

  private handleFragment() {
    this.route.fragment.pipe(take(1)).subscribe((queryFragment: any) => {
      const fragment = this.parseFragment(queryFragment);

      if (!fragment) {
        this.imgurResponseError('Invalid fragment data');
        return;
      }

      if (!this.validateTokenType(fragment.token_type)) {
        return;
      }

      if (!fragment.access_token) {
        this.imgurResponseError('Cannot get access_token from returned state!');
        return;
      }

      this.storeTokenData(fragment);

      this.finished = true;
      this.status = 'Redirecting to login...';
      setTimeout(() => {
        this.router.navigateByUrl('/login');
      }, 1250);
    });
  }

  private parseFragment(queryFragment: any) {
    const fragment: any = {};
    (new URLSearchParams(queryFragment)).forEach((value, key) => {
      fragment[key] = value;
    });
    return fragment;
  }

  private validateTokenType(tokenType: string) {
    if (
      tokenType.toLowerCase() !==
      (CONFIG['imgur.com'].authorize.token_type as string).toLowerCase()
    ) {
      this.imgurResponseError(
        'Mismatch expected and return config token type!'
      );
      console.error('Token mismatch! Expecting: ' + CONFIG['imgur.com'].authorize.token_type as string + '; Actual: ' + tokenType)
      return false;
    }
    return true;
  }

  private storeTokenData(fragment: any) {
    sessionStorage.setItem('access_token', fragment.access_token);
    sessionStorage.setItem('expires_in', fragment.expires_in);
    sessionStorage.setItem('account_username', fragment.account_username);
    localStorage.setItem('refresh_token', fragment.refresh_token);
    localStorage.setItem('account_id', fragment.account_id.toString());
  }

  private imgurResponseError(response: any | string) {
    this.matched = false;
    this.status = typeof response === 'string' ? response : this.getErrorMessage(response);
  }

  private getErrorMessage(response: any) {
    if ('error_description' in response) {
      console.error(response.error + '\n' + response.error_description + '\n' + response.error_uri);
      return response.error_description;
    } else {
      return 'Server returned ' + response.status.toString();
    }
  }
}
