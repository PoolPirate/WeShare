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
import { SharedModule } from '../shared/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ProfileMenuComponent } from './components/profile-menu/profile-menu.component';
import { RequestResetComponent } from './pages/requestreset/request-reset.component';
import { NotFoundComponent } from '../shared/pages/notfound/notfound.component';
import { ForbiddenComponent } from '../shared/pages/forbidden/forbidden.component';
import { DashboardComponent } from '../modules/dashboard/dashboard.component';
import { ProfileComponent } from '../modules/profile/profile.component';
import { ProfileUserSnippetResolver } from '../modules/profile/resolvers/profile-usersnippet.resolver';
import { ProfileProfileInfoResolver } from '../modules/profile/resolvers/profile-profileinfo.resolver';
import { ViewShareShareInfoResolver } from '../modules/share/services/resolvers/share-view-shareinfo.resolver';
import { ViewShareShareUserDataResolver } from '../modules/share/services/resolvers/share-view-shareuserdata.resolver';
import { ShareViewComponent } from '../modules/share/share-view.component';
import { ShareCreateModule } from '../modules/share-create/share-create.module';
import { SubscriptionViewSubscriptionInfoResolver } from '../modules/subscription-view/resolvers/subscription-view-subscriptioninfo.resolver';
import { SubscriptionViewComponent } from '../modules/subscription-view/subscription-view.component';
import { UserSettingsComponent } from '../modules/user-settings/user-settings.component';
import { MaterialModule } from '../modules/material/material.module';
import { ProfileStore } from '../modules/profile/services/profile-store';
import { DashboardSubscriptionSnippetsResolver } from '../modules/dashboard/resolvers/dashboard-subscriptionsnippets.resolver';
import { PostViewComponent } from '../modules/post-view/post-view.component';
import { PostViewSnippetsResolver } from '../modules/post-view/resolvers/post-view-snippets.resolver';
import { PostViewPostContentResolver } from '../modules/post-view/resolvers/post-view-post-content.resolver';

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
    loadChildren: () => import('../modules/callback/callback.module').then(m => m.CallbackModule)
  },
  {
    path: 'dashboard', component: DashboardComponent,
    loadChildren: () => import('../modules/dashboard/dashboard.module').then(m => m.DashboardModule)
  },
  {
    path: 'profile/:username', component: ProfileComponent,
    loadChildren: () => import('../modules/profile/profile.module').then(m => m.ProfileModule),
    resolve: { userSnippetResponse: ProfileUserSnippetResolver, profileInfoResponse: ProfileProfileInfoResolver }
  },
  {
    path: 'share/view/:shareId', component: ShareViewComponent, 
    loadChildren: () => import('../modules/share/share-view.module').then(m => m.ShareViewModule),
    resolve: { shareInfoResponse: ViewShareShareInfoResolver, shareUserDataResponse: ViewShareShareUserDataResolver }
  },
  { path: 'share/create', loadChildren: () => ShareCreateModule },
  {
    path: 'user/settings', component: UserSettingsComponent,
    loadChildren: () => import('../modules/user-settings/user-settings.module').then(m => m.UserSettingsModule),
  },
  {
    path: 'subscription/:subscriptionId', component: SubscriptionViewComponent,
    loadChildren: () => import('../modules/subscription-view/subscription-view.module').then(m => m.SubscriptionViewModule),
    resolve: { subscriptionInfoResponse: SubscriptionViewSubscriptionInfoResolver }
  },
  {
    path: 'post/:postId', component: PostViewComponent,
    loadChildren: () => import('../modules/post-view/post-view.module').then(m => m.PostViewModule),
    resolve: { snippetsResponse: PostViewSnippetsResolver, postContentResponse: PostViewPostContentResolver }
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
    DashboardSubscriptionSnippetsResolver,
    SubscriptionViewSubscriptionInfoResolver,
    PostViewPostContentResolver,
    PostViewSnippetsResolver,

    ProfileStore
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
