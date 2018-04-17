import { Component, OnInit, Input } from '@angular/core';
import { Goal, Timeframe } from '../../services/api/goal.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GoalFormComponent } from '../forms/goal-form/goal-form.component';
import { UserStorageService } from '../../services/user-storage.service';
import { User } from '../../services/api/user.service';

@Component({
  selector: 'app-goal-detail',
  templateUrl: './goal-detail.component.html',
  styleUrls: ['./goal-detail.component.css']
})
export class GoalDetailComponent implements OnInit {
  public timeframes = Timeframe;
  editable: boolean;
  @Input() goal: Goal;

  constructor(
    private userStorageService: UserStorageService,
    private modalService: NgbModal) { }

  ngOnInit() {
    this.editable = this.userStorageService.user.userId === this.goal.user.userId;
  }

  openGoalForm() {
    if (this.editable) {
      const modalRef = this.modalService.open(GoalFormComponent);
      modalRef.componentInstance.goal = this.goal;
    }
  }

  assignGoal() {
    //
  }
}
