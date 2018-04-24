import { Injectable } from '@angular/core';
import { User } from './user.service';
import { Goal } from './goal.service';

export class Assignment {
  assignmentId: number;
  expiration: string;
  get expirationDate(): Date {
    return new Date(this.expiration);
  }
  set expirationDate(newDate: Date) {
    this.expiration = newDate.toLocaleDateString();
  }

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

  constructor() { }

}
