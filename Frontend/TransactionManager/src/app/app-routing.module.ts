import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './pages/auth/auth.component';
import { ImgurCallbackComponent } from './pages/auth/OAuth/imgur-callback.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { ImgurComponent } from './pages/auth/OAuth/imgur.component';
import { DashboardComponent } from './pages/main/dashboard/dashboard.component';
import { authGuard } from './pages/auth/auth.guard';
const routes: Routes = [
  {
    path: '', component: AuthComponent
  },
  {
    path: 'imgur', component: ImgurComponent
  },
  {
    path: 'callback', component: ImgurCallbackComponent
  },
  {
    path: 'login', component: LoginComponent
  },
  {
    path: 'dashboard', component: DashboardComponent, canActivate: [authGuard]
  },
  { path: '', redirectTo: '/', pathMatch: 'full'},
  { path: '**', redirectTo: '/'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
