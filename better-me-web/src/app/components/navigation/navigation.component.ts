import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { UserStorageService } from '../../services/user-storage.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {
  isCollapsed: boolean;
  isAuthenticated = false;

  constructor(
    private userStorageService: UserStorageService,
    private authService: AuthService,
    private router: Router) { }

  ngOnInit() {
    this.userStorageService.isAuthenticated.subscribe(isAuthenticated => {
      this.isAuthenticated = isAuthenticated;
    });
  }

  signIn() {
    if (!this.isAuthenticated) {
      this.router.navigate(['/auth']);
    }
  }

  signOut() {
    if (this.isAuthenticated) {
      this.authService.signOut();
    }
  }
}
