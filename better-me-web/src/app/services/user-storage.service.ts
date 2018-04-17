import { Injectable } from '@angular/core';
import { User as OIDCUser } from 'oidc-client';
import { User } from './api/user.service';
import { Headers, RequestOptions } from '@angular/http';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class UserStorageService {

  private isAuthenticatedSource: Subject<boolean> = new Subject();
  public isAuthenticated: Observable<boolean> = this.isAuthenticatedSource.asObservable();

  public oidcUser: OIDCUser = null;
  public user: User = null;

  constructor() { }

  isAuthenticatedChanged() {
    this.isAuthenticatedSource.next(this.isLoggedIn());
  }

  isLoggedIn(): boolean {
    return this.oidcUser != null && !this.oidcUser.expired && this.user != null;
  }

  getAuthorizationRequestOptions() {
    return new RequestOptions({
      headers: new Headers({
        'Authorization': `${this.oidcUser.token_type} ${this.oidcUser.id_token}`
      })
    });
  }
}
