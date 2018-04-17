import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User as OIDCUser} from 'oidc-client';
import { environment } from '../../environments/environment';
import { UserService, User } from './api/user.service';
import { UserStorageService } from './user-storage.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthService {
  private manager: UserManager = new UserManager(getClientSettings());

  constructor(
    private router: Router,
    private userService: UserService,
    private userStorageService: UserStorageService) { }

  private getUser(oidcUser: OIDCUser): Promise<User> {
    if (oidcUser != null) {
      return this.userService
        .getUsersByEmail(oidcUser.profile.email)
        .then(users => {
          return users.length === 1 ? users[0] : null;
        });
    }
    return Promise.resolve(null);
  }

  private storeUser(oidcUser: OIDCUser): Promise<void> {
    this.userStorageService.oidcUser = oidcUser;
    return this.getUser(oidcUser).then(user => {
      this.userStorageService.user = user;
      this.userStorageService.isAuthenticatedChanged();
    });
  }

  isLoggedIn(): Promise<boolean> {
    return this.manager.getUser().then((oidcUser) => {
      return this.storeUser(oidcUser).then(() => {
        return this.userStorageService.isLoggedIn();
      });
    });
  }

  startAuthentication(): Promise<void> {
    return this.manager.signinRedirect();
  }

  completeAuthentication(): Promise<void> {
    return this.manager.signinRedirectCallback().then((oidcUser) => {
      this.storeUser(oidcUser).then(() => {
        this.router.navigate(['/home']);
      });
    });
  }

  signOut(): Promise<void> {
    return this.manager.removeUser().then(() => {
      this.storeUser(null).then(() => {
        this.router.navigate(['/login']);
      });
    });
  }
}

export function getClientSettings(): UserManagerSettings {
  return {
    authority: 'https://accounts.google.com/',
    client_id: '901912659612-fqefb0429a6l0ruc931nlbluf3g04i3o.apps.googleusercontent.com',
    redirect_uri: `${environment.baseUrl}/auth-callback`,
    post_logout_redirect_uri: `${environment.baseUrl}`,
    response_type: 'id_token token',
    scope: 'openid profile email',
    filterProtocolClaims: true,
    loadUserInfo: true,
    automaticSilentRenew: true,
    silent_redirect_uri: `${environment.baseUrl}/silent-refresh.html`
  };
}
