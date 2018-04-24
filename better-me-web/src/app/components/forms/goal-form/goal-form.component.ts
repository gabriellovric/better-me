import { Component, OnInit, Input } from '@angular/core';
import { Goal, GoalService, Timeframe } from '../../../services/api/goal.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Achievement, AchievementService } from '../../../services/api/achievement.service';

@Component({
  selector: 'app-goal-form',
  templateUrl: './goal-form.component.html',
  styleUrls: ['./goal-form.component.css']
})
export class GoalFormComponent implements OnInit {
  @Input() goal: Goal;
  @Input() achievements: Achievement[];

  public timeframes = Timeframe;
  public keys = Object.keys(this.timeframes).filter(Number);

  constructor(
    private goalService: GoalService,
    private achievementService: AchievementService,
    public activeModal: NgbActiveModal) { }

  ngOnInit() {
  }

  private syncGoals(): Promise<boolean> {
    if (this.goal.goalId === 0) {
      return this.goalService
        .createGoal(this.goal)
        .then(goal => {
          this.goal = goal;
          return true;
        }, reason => {
          return false;
        });
    } else {
      return this.goalService
        .editGoal(this.goal);
    }
  }

  private syncAchievements(): Promise<boolean[]> {
    const promise_array = [];

    for (let i = 0; i < this.achievements.length; i++) {
      if (this.achievements[i].achievementId === 0) {
        this.achievements[i].goalId = this.goal.goalId;
        this.achievements[i].goal = null;

        promise_array.push(
          this.achievementService
            .createAchievement(this.achievements[i])
            .then(achievement => {
              this.achievements[i] = achievement;
              return true;
            }, reason => {
              return false;
            })
        );
      } else {
        promise_array.push(
          this.achievementService
            .editAchievement(this.achievements[i])
        );
      }
    }

    return Promise.all<boolean>(promise_array);
  }

  onSubmit(): void {
    this
      .syncGoals()
      .then(succeeded => {
        if (succeeded) {
          return this.syncAchievements();
        } else {
          console.error('Failed to sync goal');
        }
      })
      .then(succeeded => {
        if (succeeded.every(b => b)) {
          this.activeModal.close(this.goal);
        } else {
          console.error('Failed to sync achievements');
        }
      });
  }

  addAchievement() {
    this.achievements.push(new Achievement());
  }
}
