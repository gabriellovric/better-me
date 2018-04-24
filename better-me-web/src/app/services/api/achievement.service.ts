import { Injectable } from '@angular/core';
import { Goal } from './goal.service';
import { environment } from '../../../environments/environment';
import { Http } from '@angular/http';
import { UserStorageService } from '../user-storage.service';


export class Achievement {
  achievementId: number;
  name: string;
  description: string;
  achieved: number;
  goalId: number;
  goal: Goal;

  constructor() {
    this.achievementId = 0;
  }
}

@Injectable()
export class AchievementService {
  private baseUrl = `${environment.apiUrl}/achievements`;

  constructor(private http: Http, private userStorageService: UserStorageService) { }

  getAchievements(): Promise<Achievement[]> {
    return this.http
      .get(`${this.baseUrl}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Achievement[])
      .toPromise();
  }

  getAchievementsByGoal(goalId: number): Promise<Achievement[]> {
    return this.http
      .get(`${this.baseUrl}?goalId=${goalId}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Achievement[])
      .toPromise();
  }

  getAchievement(id: number): Promise<Achievement> {
    return this.http
      .get(`${this.baseUrl}/${id}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Achievement)
      .toPromise();
  }

  createAchievement(achievement: Partial<Achievement>): Promise<Achievement> {
    return this.http
      .post(`${this.baseUrl}`, achievement, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Achievement)
      .toPromise();
  }

  editAchievement(achievement: Achievement): Promise<boolean> {
    return this.http
    .put(`${this.baseUrl}/${achievement.achievementId}`, Achievement, this.userStorageService.getAuthorizationRequestOptions())
    .map(response => response.ok)
    .toPromise();
  }

  deleteAchievement(achievementId: number): Promise<boolean> {
    return this.http
    .delete(`${this.baseUrl}/${achievementId}`, this.userStorageService.getAuthorizationRequestOptions())
    .map(response => response.ok)
    .toPromise();
  }
}
