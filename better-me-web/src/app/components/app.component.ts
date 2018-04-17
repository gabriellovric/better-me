import { Component, OnInit } from '@angular/core';
import { UserStorageService } from '../services/user-storage.service';
import 'rxjs/add/operator/skip';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';

  constructor() { }

  ngOnInit() {
  }
}
