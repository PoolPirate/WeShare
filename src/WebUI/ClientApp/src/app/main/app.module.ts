import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ProfileMenuComponent } from './components/profile-menu/profile-menu.component';
import { DashboardComponent } from '../modules/dashboard/dashboard.component';
import { ProfileComponent } from '../modules/profile/profile.component';
import { ProfileUserSnippetResolver } from '../modules/profile/services/resolvers/profile-usersnippet.resolver';
import { ProfileProfileInfoResolver } from '../modules/profile/services/resolvers/profile-profileinfo.resolver';
import { ViewShareShareInfoResolver } from '../modules/share/services/resolvers/share-view-shareinfo.resolver';
import { ViewShareShareUserDataResolver } from '../modules/share/services/resolvers/share-view-shareuserdata.resolver';
import { ShareViewComponent } from '../modules/share/share-view.component';
import { SubscriptionViewSubscriptionInfoResolver } from '../modules/subscription-view/services/resolvers/subscription-view-subscriptioninfo.resolver';
import { SubscriptionViewComponent } from '../modules/subscription-view/subscription-view.component';
import { UserSettingsComponent } from '../modules/user-settings/user-settings.component';
import { ProfileStore } from '../modules/profile/services/profile-store';
import { DashboardSubscriptionSnippetsResolver } from '../modules/dashboard/resolvers/dashboard-subscriptionsnippets.resolver';
import { PostViewComponent } from '../modules/post-view/post-view.component';
import { PostViewSnippetsResolver } from '../modules/post-view/resolvers/post-view-snippets.resolver';
import { PostViewPostContentResolver } from '../modules/post-view/resolvers/post-view-post-content.resolver';
import { AuthService } from '../../services/authservice';
import { WeShareClient } from '../../services/weshareclient';
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { NotFoundComponent } from './pages/notfound/notfound.component';
import { ForbiddenComponent } from './pages/forbidden/forbidden.component';
import { BrowserModule } from '@angular/platform-browser';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'notfound', component: NotFoundComponent },
  { path: 'forbidden', component: ForbiddenComponent },
  { path: 'login', redirectTo: 'account/login' },
  {
    path: 'account',
    loadChildren: () => import('../modules/account/account.module').then(m => m.AccountModule)
  },
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
    DashboardComponent,
    HomeComponent,
    NotFoundComponent,
    ForbiddenComponent,
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    RouterModule.forRoot(routes),
    LoadingBarRouterModule,
    MatIconModule,
    MatMenuModule,
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

    ProfileStore,
    AuthService,
    WeShareClient
  ],
  exports: [
    RouterModule,
    LoadingBarRouterModule,
    MatIconModule,
    MatMenuModule,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
