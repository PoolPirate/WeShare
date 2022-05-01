import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../../../material/material.module';
import { SharedModule } from '../../../../shared/shared.module';
import { SubscriptionViewPostsPendingPage } from './pages/pending/subscription-view-posts-pending.page';
import { SubscriptionViewPostsUnsentPage } from './pages/unsent/subscription-view-posts-unsent.page';
import { SubscriptionViewPostsReceivedPage } from './pages/received/subscription-view-posts-received.page';
import { SubscriptionViewPostsNavMenuComponent } from './components/nav-menu/subscription-view-postsnav-menu.component';
import { SubscriptionViewPostsComponent } from './subscription-view-posts.component';
import { SubscriptionViewPostsUnsentResolver } from './services/resolvers/subscription-view-posts-unsent.resolver';
import { SubscriptionViewPostsPendingResolver } from './services/resolvers/subscription-view-posts-pending.resolver';
import { SubscriptionViewPostsReceivedResolver } from './services/resolvers/subscription-view-posts-received.resolver';


const routes: Routes = [
  { path: '', redirectTo: 'pending' },
  {
    path: 'unsent', component: SubscriptionViewPostsUnsentPage,
    resolve: { unsentResponse: SubscriptionViewPostsUnsentResolver }
  },
  {
    path: 'pending', component: SubscriptionViewPostsPendingPage,
    resolve: { pendingResponse: SubscriptionViewPostsPendingResolver }
  },
  {
    path: 'received', component: SubscriptionViewPostsReceivedPage,
    resolve: { receivedResponse: SubscriptionViewPostsReceivedResolver }
  }
]

@NgModule({
  declarations: [
    SubscriptionViewPostsComponent,

    SubscriptionViewPostsUnsentPage,
    SubscriptionViewPostsPendingPage,
    SubscriptionViewPostsReceivedPage,

    SubscriptionViewPostsNavMenuComponent
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ],
  providers: [
    SubscriptionViewPostsUnsentResolver,
    SubscriptionViewPostsPendingResolver,
    SubscriptionViewPostsReceivedResolver,
  ]
})
export class SubscriptionViewPostsModule { }
