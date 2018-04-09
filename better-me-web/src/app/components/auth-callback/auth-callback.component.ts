import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { UserService, User } from '../../services/api/user.service';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router) { }

  ngOnInit() {
      this.authService.completeAuthentication()
      .then(() => this.router.navigate(['/']));
        //.catch((reason: any) => this.router.navigate(['/'])); todo add error view
  }
}
