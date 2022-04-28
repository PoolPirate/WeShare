import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ProfileNavMenuComponent } from './nav-menu/profile-nav-menu.component';
import { ProfileOverviewComponent } from './pages/overview/profile-overview.component';
import { SharedModule } from '../../shared/shared.module';
import { ProfileSharesComponent } from './pages/shares/profile-shares.component';
import { MaterialModule } from '../material/material.module';
import { ProfilePopularSharesResolver } from './resolvers/profile-popular-shares.resolver';
import { ProfileSharesResolver } from './resolvers/profile-shares.resolver';
import { ProfileComponent } from './profile.component';
import { ProfileLikesResolver } from './resolvers/profile-likes.resolver';
import { ProfileLikesComponent } from './pages/likes/profile-likes.component';
import { ProfileSubscriptionsComponent } from './pages/subscriptions/profile-subscriptions.component';
import { ProfileSubscriptionSnippetsResolver } from './resolvers/profile-subscriptionsnippets.resolver';

const routes: Routes = [
  { path: '', redirectTo: 'overview' },
  {
    path: 'overview', component: ProfileOverviewComponent,
    resolve: { popularSharesResponse: ProfilePopularSharesResolver  }
  },
  {
    path: 'shares', component: ProfileSharesComponent,
    resolve: { sharesResponse: ProfileSharesResolver }
  },
  {
    path: 'likes', component: ProfileLikesComponent,
    resolve: { likesResponse: ProfileLikesResolver },
  },
  {
    path: 'subscriptions', component: ProfileSubscriptionsComponent,
    resolve: { subscriptionSnippetsResponse: ProfileSubscriptionSnippetsResolver }
  }
]

@NgModule({
  declarations: [
    ProfileComponent,
    ProfileNavMenuComponent,
    ProfileOverviewComponent,
    ProfileSharesComponent,
    ProfileLikesComponent,
    ProfileSubscriptionsComponent,
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes)
  ],
  providers: [
    ProfilePopularSharesResolver,
    ProfileSharesResolver,
    ProfileLikesResolver,
    ProfileSubscriptionSnippetsResolver
  ]
})
export class ProfileModule { }
