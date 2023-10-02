import { Component } from '@angular/core';
import { LoginComponent } from './pages/login/login.component';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'Transaction Manager';
  currentRoute: string = '';
  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    // Subscribe to route changes
    console.log(this.currentRoute)
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        // Get the current route
        this.currentRoute = this.activatedRoute?.firstChild?.snapshot?.routeConfig?.path || '';
      });
  }
}
