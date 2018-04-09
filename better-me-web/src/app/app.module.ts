import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { fab } from '@fortawesome/free-brands-svg-icons';

library.add(fas, far, fab);

import { AppComponent } from './components/app.component';
import { HomeComponent } from './components/home/home.component';
import { ExploreComponent } from './components/explore/explore.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NavigationComponent } from './components/navigation/navigation.component';
import { StatisticComponent } from './components/statistic/statistic.component';
import { ProfileComponent } from './components/profile/profile.component';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { LoginComponent } from './components/login/login.component';

import { GuestGuardService } from './services/guest-guard.service';
import { AuthGuardService } from './services/auth-guard.service';
import { AuthService } from './services/auth.service';
import { AuthComponent } from './components/auth/auth.component';
import { UserService } from './services/api/user.service';
import { GoalDetailComponent } from './components/goal-detail/goal-detail.component';
import { GoalService } from './services/api/goal.service';
import { AchievementService } from './services/api/achievement.service';
import { AssignmentService } from './services/api/assignment.service';
import { ProgressService } from './services/api/progress.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ExploreComponent,
    DashboardComponent,
    NavigationComponent,
    StatisticComponent,
    ProfileComponent,
    AuthCallbackComponent,
    LoginComponent,
    AuthComponent,
    GoalDetailComponent,
  ],
  imports: [
    HttpModule,
    BrowserModule,
    AppRoutingModule,
    FontAwesomeModule,
    NgbModule.forRoot(),
  ],
  providers: [
    GuestGuardService,
    AuthGuardService,
    AuthService,
    UserService,
    GoalService,
    AchievementService,
    AssignmentService,
    ProgressService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
