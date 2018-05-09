import { Injectable } from '@angular/core';
import { User } from './user.service';
import { Goal } from './goal.service';
import { environment } from '../../../environments/environment';
import { Http } from '@angular/http';
import { UserStorageService } from '../user-storage.service';

export class Assignment {
  assignmentId: number;
  expiration: string;
  /*get expirationDate(): Date {
    return new Date(this.expiration);
  }
  set expirationDate(newDate: Date) {
    this.expiration = newDate.toLocaleDateString();
  }*/
  userId: number;
  user: User;
  goalId: number;
  goal: Goal;

  constructor() {
    //
  }
}

@Injectable()
export class AssignmentService {
  private baseUrl = `${environment.apiUrl}/assignments`;

  constructor(private http: Http, private userStorageService: UserStorageService) { }

  getAssignments(): Promise<Assignment[]> {
    return this.http
      .get(`${this.baseUrl}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Assignment[])
      .toPromise();
  }

  getAssignmentsByUser(userId: number): Promise<Assignment[]> {
    return this.http
      .get(`${this.baseUrl}?userId=${userId}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Assignment[])
      .toPromise();
  }

  getAssignmentsByGoal(goalId: number): Promise<Assignment[]> {
    return this.http
      .get(`${this.baseUrl}?goalId=${goalId}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Assignment[])
      .toPromise();
  }

  getAssignment(id: number): Promise<Assignment> {
    return this.http
      .get(`${this.baseUrl}/${id}`, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Assignment)
      .toPromise();
  }

  createAssignment(assignment: Partial<Assignment>): Promise<Assignment> {
    return this.http
      .post(`${this.baseUrl}`, assignment, this.userStorageService.getAuthorizationRequestOptions())
      .map(response => response.json() as Assignment)
      .toPromise();
  }

  editAssignment(assignment: Assignment): Promise<boolean> {
    return this.http
    .put(`${this.baseUrl}/${assignment.assignmentId}`, assignment, this.userStorageService.getAuthorizationRequestOptions())
    .map(response => response.ok)
    .toPromise();
  }

  deleteAssignment(assignmentId: number): Promise<boolean> {
    return this.http
    .delete(`${this.baseUrl}/${assignmentId}`, this.userStorageService.getAuthorizationRequestOptions())
    .map(response => response.ok)
    .toPromise();
  }
}
