import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User} from 'oidc-client';
import { Headers, RequestOptions } from '@angular/http';
import { environment } from '../../environments/environment';

@Injectable()
export class AuthService {
  private manager: UserManager = new UserManager(getClientSettings());
  private user: User = null;
  
  public ready: Promise<void>;

  constructor() {
    this.ready = this.manager.getUser().then(user => {
      this.user = user;
    });
  }

  isLoggedIn(): boolean {
    return this.user != null && !this.user.expired;
  }

  getClaims(): any {
    return this.user.profile;
  }

  getAuthorizationRequestOptions() {
    return new RequestOptions({
      headers: new Headers({
        'Authorization': `${this.user.token_type} ${this.user.id_token}`
      })
    });
  }

  startAuthentication(): Promise<void> {
    return this.manager.signinRedirect();
  }

  completeAuthentication(): Promise<void> {
    return this.manager.signinRedirectCallback().then(user => {
      this.user = user;
    });
  }
}

export function getClientSettings(): UserManagerSettings {
  return {
    authority: 'https://accounts.google.com/',
    client_id: '901912659612-fqefb0429a6l0ruc931nlbluf3g04i3o.apps.googleusercontent.com',
    redirect_uri: `${environment.baseUrl}/auth-callback`,
    post_logout_redirect_uri: `${environment.baseUrl}`,
    response_type: "id_token token",
    scope: "openid profile email",
    filterProtocolClaims: true,
    loadUserInfo: true,
    automaticSilentRenew: true,
    silent_redirect_uri: `${environment.baseUrl}/silent-refresh.html`
  };
}
