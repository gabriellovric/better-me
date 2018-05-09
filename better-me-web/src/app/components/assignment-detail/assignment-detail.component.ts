import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Assignment, AssignmentService } from '../../services/api/assignment.service';
import { Timeframe } from '../../services/api/goal.service';
import { Progress, ProgressService } from '../../services/api/progress.service';

@Component({
  selector: 'app-assignment-detail',
  templateUrl: './assignment-detail.component.html',
  styleUrls: ['./assignment-detail.component.css']
})
export class AssignmentDetailComponent implements OnInit {
  timeframes = Timeframe;
  @Input() assignment: Assignment;
  @Output() assignmentChanged = new EventEmitter<boolean>();
  progresses: Progress[];

  constructor(
    private progressService: ProgressService,
    private assignmentService: AssignmentService
  ) { }

  ngOnInit() {
    this.progressService.getProgressesByAssignment(this.assignment.assignmentId)
      .then(progresses => this.progresses = progresses);
  }

  increment() {
    this.progressService.createProgress({
      assignmentId: this.assignment.assignmentId
    })
    .then(progress => {
      this.progresses.push(progress);
    });
  }

  unassign() {
    this.assignmentService.deleteAssignment(this.assignment.assignmentId)
    .then(success => {
      this.assignmentChanged.emit(true);
    });
  }
}
