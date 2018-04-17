import { Component, OnInit } from '@angular/core';
import { UserStorageService } from '../../services/user-storage.service';
import { User } from '../../services/api/user.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GoalFormComponent } from '../forms/goal-form/goal-form.component';
import { Goal } from '../../services/api/goal.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  user: User;
  constructor(
    private userStorageService: UserStorageService,
    private modalService: NgbModal) { }

  ngOnInit() {
    this.user = this.userStorageService.user;
  }

  openGoalForm() {
    const modalRef = this.modalService.open(GoalFormComponent);
    modalRef.componentInstance.goal = new Goal();
  }

}
