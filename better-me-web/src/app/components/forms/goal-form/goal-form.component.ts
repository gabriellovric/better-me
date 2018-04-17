import { Component, OnInit, Input } from '@angular/core';
import { Goal, GoalService, Timeframe } from '../../../services/api/goal.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-goal-form',
  templateUrl: './goal-form.component.html',
  styleUrls: ['./goal-form.component.css']
})
export class GoalFormComponent implements OnInit {
  @Input() goal: Goal;
  public timeframes = Timeframe;
  public keys = Object.keys(this.timeframes).filter(Number);

  constructor(
    private goalService: GoalService,
    public activeModal: NgbActiveModal) { }

  ngOnInit() {
  }

  onSubmit(): void {
    if (this.goal.goalId === 0) {
      this.goalService.createGoal(this.goal);
    } else {
      //
    }
  }

}
