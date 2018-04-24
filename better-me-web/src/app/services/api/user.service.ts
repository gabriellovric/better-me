import { Injectable, Inject, forwardRef } from '@angular/core';
import { Http } from '@angular/http';
import { UserStorageService } from '../user-storage.service';
import { environment } from '../../../environments/environment';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

export class User {
  userId: number;
  firstname: string;
  lastname: string;
  email: string;
}

@Injectable()
export class UserService {

  private baseUrl = `${environment.apiUrl}/users`;

  constructor(private http: Http, private userStorageService: UserStorageService) { }

  getUsers(): Promise<User[]> {
    return this.http
      .get(`${this.baseUrl}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as User[])
      .toPromise();
  }

  getUsersByEmail(email: string): Promise<User[]> {
    return this.http
      .get(`${this.baseUrl}?email=${email}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as User[])
      .toPromise();
  }

  getUser(id: number): Promise<User> {
    return this.http
      .get(`${this.baseUrl}/${id}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as User)
      .toPromise();
  }

  createUser(user: Partial<User>): Promise<User> {
    return this.http
      .post(`${this.baseUrl}`, user, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as User)
      .toPromise();
  }
}
