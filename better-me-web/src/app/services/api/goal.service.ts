import { Injectable } from '@angular/core';
import { User } from './user.service';
import { Http } from '@angular/http';
import { AuthService } from '../auth.service';
import { environment } from '../../../environments/environment';


import 'rxjs/add/operator/map'
import 'rxjs/add/operator/toPromise';

export class Goal {
  goalId: number;
  user: User;
  name: string;
  description: string;
  timeframe: string;
  timeframeText: string;
  quantity: number;
}

@Injectable()
export class GoalService {

  private baseUrl = `${environment.apiUrl}/goals`;

  constructor(private http: Http, private authService: AuthService) { }

  getGoals(): Promise<Goal[]> {
    return this.http
      .get(`${this.baseUrl}`, this.authService.getAuthorizationRequestOptions())
      .map(response => response.json() as Goal[])
      .toPromise();
  }

  getGoalsByUser(userId: number): Promise<Goal[]> {
    return this.http
      .get(`${this.baseUrl}?userId=${userId}`, this.authService.getAuthorizationRequestOptions())
      .map(response => response.json() as Goal[])
      .toPromise();
  }

  getGoal(id: number): Promise<Goal> {
    return this.http
      .get(`${this.baseUrl}/${id}`, this.authService.getAuthorizationRequestOptions())
      .map(response => response.json() as Goal)
      .toPromise();
  }

  createGoal(user: Partial<Goal>): Promise<Goal> {
    return this.http
      .post(`${this.baseUrl}`, user, this.authService.getAuthorizationRequestOptions())
      .map(response => response.json() as Goal)
      .toPromise();
  }
}
