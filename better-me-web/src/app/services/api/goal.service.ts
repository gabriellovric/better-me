import { Injectable } from '@angular/core';
import { User } from './user.service';
import { Http } from '@angular/http';
import { UserStorageService } from '../user-storage.service';
import { environment } from '../../../environments/environment';


import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

export enum Timeframe {
  Hour,
  Day,
  Week,
  Month,
  Year
}

export class Goal {
  goalId: number;
  user: User;
  name: string;
  description: string;
  timeframe: Timeframe;
  repetitions: number;

  constructor() {
    this.goalId = 0;
  }
}

@Injectable()
export class GoalService {

  private baseUrl = `${environment.apiUrl}/goals`;

  constructor(private http: Http, private userStorageService: UserStorageService) { }

  getGoals(): Promise<Goal[]> {
    return this.http
      .get(`${this.baseUrl}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Goal[])
      .toPromise();
  }

  getGoalsByUser(userId: number): Promise<Goal[]> {
    return this.http
      .get(`${this.baseUrl}?userId=${userId}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Goal[])
      .toPromise();
  }

  getGoal(id: number): Promise<Goal> {
    return this.http
      .get(`${this.baseUrl}/${id}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Goal)
      .toPromise();
  }

  createGoal(user: Partial<Goal>): Promise<Goal> {
    return this.http
      .post(`${this.baseUrl}`, user, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Goal)
      .toPromise();
  }
}
