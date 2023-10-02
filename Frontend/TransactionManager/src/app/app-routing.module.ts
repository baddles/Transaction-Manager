import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { CallbackComponent } from './pages/login/OAuth/OAuth_Callback/callback.component';
import { ImgurComponent } from './pages/login/OAuth/imgur.component';
const routes: Routes = [
  {
    path: '', component: ImgurComponent
  },
  {
    path: 'callback', component: CallbackComponent
  },
  {
    path: 'login', component: LoginComponent
  },
  { path: '', redirectTo: '/', pathMatch: 'full'},
  { path: '**', redirectTo: 'login'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
