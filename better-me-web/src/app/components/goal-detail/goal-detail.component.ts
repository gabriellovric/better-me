import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Goal, Timeframe } from '../../services/api/goal.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GoalFormComponent } from '../forms/goal-form/goal-form.component';
import { UserStorageService } from '../../services/user-storage.service';
import { User } from '../../services/api/user.service';
import { AchievementService, Achievement } from '../../services/api/achievement.service';
import { AssignmentService, Assignment } from '../../services/api/assignment.service';

@Component({
  selector: 'app-goal-detail',
  templateUrl: './goal-detail.component.html',
  styleUrls: ['./goal-detail.component.css']
})
export class GoalDetailComponent implements OnInit {
  public timeframes = Timeframe;
  editable: boolean;
  isAssigned: boolean;
  @Input() goal: Goal;
  @Output() assignmentChanged = new EventEmitter<boolean>();
  achievements: Achievement[];
  assignment: Assignment;

  constructor(
    private userStorageService: UserStorageService,
    private achievementService: AchievementService,
    private assignmentService: AssignmentService,
    private modalService: NgbModal) { }

  ngOnInit() {
    this.achievementService
      .getAchievementsByGoal(this.goal.goalId)
      .then(achievements => this.achievements = achievements);

    this.assignmentService
      .getAssignmentsByGoal(this.goal.goalId)
      .then(assignments => this.assignment = assignments.length === 1 ? assignments[0] : null);

    this.editable = this.userStorageService.user.userId === this.goal.user.userId;
  }

  openGoalForm() {
    if (this.editable) {
      const modalRef = this.modalService.open(GoalFormComponent);
      modalRef.componentInstance.goal = this.goal;
      modalRef.componentInstance.achievements = this.achievements;
    }
  }

  assignGoal() {
    this.assignmentService.createAssignment({
      goalId: this.goal.goalId
    })
    .then(assignment => {
      this.assignment = assignment;
      this.assignmentChanged.emit(true);
    });
  }

  unassignGoal() {
    this.assignmentService.deleteAssignment(this.assignment.assignmentId)
    .then(success => {
      this.assignment = null;
      this.assignmentChanged.emit(true);
    });
  }
}
