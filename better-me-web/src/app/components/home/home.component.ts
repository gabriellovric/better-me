import { Component, OnInit } from '@angular/core';
import { UserStorageService } from '../../services/user-storage.service';
import { User } from '../../services/api/user.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GoalFormComponent } from '../forms/goal-form/goal-form.component';
import { Goal, GoalService } from '../../services/api/goal.service';
import { Subject } from 'rxjs/Subject';
import { debounceTime } from 'rxjs/operator/debounceTime';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  private success = new Subject<string>();

  successMessage: string;
  myGoals: Goal[];
  user: User;

  constructor(
    private userStorageService: UserStorageService,
    private goalService: GoalService,
    private modalService: NgbModal) { }

  ngOnInit() {
    this.user = this.userStorageService.user;
    this.goalService.getGoalsByUser(this.user.userId).then(goals => this.myGoals = goals);

    this.success.subscribe((message) => this.successMessage = message);
    debounceTime.call(this.success, 5000).subscribe(() => this.successMessage = null);
  }

  openGoalForm() {
    const modalRef = this.modalService.open(GoalFormComponent, { size: 'lg' });
    modalRef.componentInstance.goal = new Goal();
    modalRef.componentInstance.achievements = [];
    modalRef.result.then(result => {
      const goal = result as Goal;
      if (goal != null && goal.goalId != null) {
        this.success.next(`The Goal: ${goal.name}, was successfully created.`);
      }
    });
  }

}
