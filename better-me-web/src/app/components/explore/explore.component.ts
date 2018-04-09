import { Component, OnInit } from '@angular/core';
import { GoalService, Goal } from '../../services/api/goal.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-explore',
  templateUrl: './explore.component.html',
  styleUrls: ['./explore.component.css']
})
export class ExploreComponent implements OnInit {

  goals: Goal[];

  constructor(private goalService: GoalService, private authService: AuthService) { }

  ngOnInit() {
    this.goalService.createGoal({
      name: "Goal 1",
      description: "Sometimes you want to rename in 1 file, and other times you want to rename across multiple files.",
      timeframe: "Day",
      quantity: 5
    })
    .then(() =>
      this.goalService.createGoal({
        name: "Goal 2",
        description: "Sometimes you want to rename in 1 file, and other times you want to rename across multiple files.",
        timeframe: "Day",
        quantity: 5
      })
    )
    .then(() =>
      this.goalService.createGoal({
        name: "Goal 3",
        description: "Sometimes you want to rename in 1 file, and other times you want to rename across multiple files.",
        timeframe: "Day",
        quantity: 5
      })
    )
    .then(() =>
      this.goalService.createGoal({
        name: "Goal 4",
        description: "Sometimes you want to rename in 1 file, and other times you want to rename across multiple files.",
        timeframe: "Day",
        quantity: 5
      })
    )
    .then(() =>
      this.goalService.createGoal({
        name: "Goal 5",
        description: "Sometimes you want to rename in 1 file, and other times you want to rename across multiple files.",
        timeframe: "Day",
        quantity: 5
      })
    )
    .then(() =>
      this.goalService.createGoal({
        name: "Goal 6",
        description: "Sometimes you want to rename in 1 file, and other times you want to rename across multiple files.",
        timeframe: "Day",
        quantity: 5
      })
    )
    .then(() =>
      this.goalService.createGoal({
        name: "Goal 7",
        description: "Sometimes you want to rename in 1 file, and other times you want to rename across multiple files.",
        timeframe: "Day",
        quantity: 5
      })
    )
    .then(() => this.goalService.getGoals())
    .then(goals => this.goals = goals);
  }

}
