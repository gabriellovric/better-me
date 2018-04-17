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
    this.goalService.getGoals()
    .then(goals => this.goals = goals);
  }

}
