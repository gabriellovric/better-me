import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthGuardService } from './services/auth-guard.service';
import { GuestGuardService } from './services/guest-guard.service';

import { HomeComponent } from './components/home/home.component';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { LoginComponent } from './components/login/login.component';
import { AuthComponent } from './components/auth/auth.component';
import { ExploreComponent } from './components/explore/explore.component';


const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [GuestGuardService] },
  { path: 'auth', component: AuthComponent, canActivate: [GuestGuardService] },
  { path: 'auth-callback', component: AuthCallbackComponent, canActivate: [GuestGuardService] },

  { path: 'home', component: HomeComponent, canActivate: [AuthGuardService] },
  { path: 'explore', component: ExploreComponent, canActivate: [AuthGuardService] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
