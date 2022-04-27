import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { LogoutComponent } from './pages/logout/logout.component';
import { RegisterComponent } from './pages/register/register.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { NotFoundComponent } from '../shared/components/notfound/notfound.component';
import { SharedModule } from '../shared/shared.module';
import { ShareCreateModule } from '../share-create/share-create.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ProfileComponent } from '../profile/profile.component';
import { ShareViewComponent } from '../share-view/share-view.component';
import { MaterialModule } from '../material/material.module';
import { ProfileMenuComponent } from './components/profile-menu/profile-menu.component';
import { RequestResetComponent } from './pages/requestreset/request-reset.component';
import { UserSettingsComponent } from '../user-settings/user-settings.component';
import { ViewShareShareInfoResolver } from '../share-view/resolvers/share-view-shareinfo.resolver';
import { ViewShareShareUserDataResolver } from '../share-view/resolvers/share-view-shareuserdata.resolver';
import { ProfileUserSnippetResolver } from '../profile/resolvers/profile-usersnippet.resolver';
import { ProfileProfileInfoResolver } from '../profile/resolvers/profile-profileinfo.resolver';
import { ProfileStore } from '../profile/services/profile-store';
import { ForbiddenComponent } from '../shared/components/forbidden/forbidden.component';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { DashboardSubscriptionInfosResolver } from '../dashboard/resolvers/dashboard-subscriptioninfos.resolver';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'notfound', component: NotFoundComponent },
  { path: 'forbidden', component: ForbiddenComponent },
  { path: 'login', component: LoginComponent },
  { path: 'requestreset', component: RequestResetComponent },
  { path: 'logout', component: LogoutComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'c',
    loadChildren: () => import('../callback/callback.module').then(m => m.CallbackModule)
  },
  {
    path: 'dashboard', component: DashboardComponent,
    loadChildren: () => import('../dashboard/dashboard.module').then(m => m.DashboardModule)
  },
  {
    path: 'profile/:username', component: ProfileComponent,
    loadChildren: () => import('../profile/profile.module').then(m => m.ProfileModule),
    resolve: { userSnippetResponse: ProfileUserSnippetResolver, profileInfoResponse: ProfileProfileInfoResolver }
  },
  {
    path: 'share/view/:shareId', component: ShareViewComponent, 
    loadChildren: () => import('../share-view/share-view.module').then(m => m.ShareViewModule),
    resolve: { shareInfoResponse: ViewShareShareInfoResolver, shareUserDataResponse: ViewShareShareUserDataResolver }
  },
  { path: 'share/create', loadChildren: () => ShareCreateModule },
  {
    path: 'user/settings', component: UserSettingsComponent,
    loadChildren: () => import('../user-settings/user-settings.module').then(m => m.UserSettingsModule),
  }
]

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ProfileMenuComponent,
    RequestResetComponent,
    DashboardComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    LogoutComponent,
  ],
  imports: [
    MaterialModule,
    BrowserAnimationsModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    RouterModule.forRoot(routes),
    SharedModule
  ],
  providers: [
    ViewShareShareInfoResolver,
    ViewShareShareUserDataResolver,
    ProfileUserSnippetResolver,
    ProfileProfileInfoResolver,
    DashboardSubscriptionInfosResolver,
    ProfileStore
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
