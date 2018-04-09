import { Injectable, Inject, forwardRef } from '@angular/core';
import { Http } from '@angular/http';
import { AuthService } from '../auth.service';
import { environment } from '../../../environments/environment';

import 'rxjs/add/operator/map'
import 'rxjs/add/operator/toPromise';

export class User {
  id: number;
  firstname: string;
  lastname: string;
  email: string;
}

@Injectable()
export class UserService {

  private baseUrl = `${environment.apiUrl}/users`;

  constructor(private http: Http, private authService: AuthService) { }

  getUsers(): Promise<User[]> {
    return this.http
      .get(`${this.baseUrl}`, this.authService.getAuthorizationRequestOptions())
      .map(response => response.json() as User[])
      .toPromise();
  }

  getUsersByEmail(email: string): Promise<User[]> {
    return this.http
      .get(`${this.baseUrl}?email=${email}`, this.authService.getAuthorizationRequestOptions())
      .map(response => response.json() as User[])
      .toPromise();
  }

  getUser(id: number): Promise<User> {
    return this.http
      .get(`${this.baseUrl}/${id}`, this.authService.getAuthorizationRequestOptions())
      .map(response => response.json() as User)
      .toPromise();
  }

  createUser(user: Partial<User>): Promise<User> {
    return this.http
      .post(`${this.baseUrl}`, user, this.authService.getAuthorizationRequestOptions())
      .map(response => response.json() as User)
      .toPromise();
  }

}
