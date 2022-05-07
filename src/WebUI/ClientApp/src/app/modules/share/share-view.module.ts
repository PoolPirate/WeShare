import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ReactiveFormsModule } from '@angular/forms';
import { ShareViewComponent } from './share-view.component';
import { ShareViewNavMenuComponent } from './components/nav-menu/share-view-nav-menu.component';
import { ShareViewOverviewComponent } from './pages/overview/share-view-overview.component';
import { ShareViewPostsComponent } from './pages/posts/share-view-posts.component';
import { MaterialModule } from '../material/material.module';
import { ViewSharePostsResolver } from './services/resolvers/share-view-posts.resolver';
import { ViewShareSecretsResolver } from './services/resolvers/share-view-secrets.resolver';
import { ShareViewSubscribeButtonComponent } from './components/subscribe-button/share-view-subscribe-button.component';
import { ShareViewSubscriptionsComponent } from './pages/subscriptions/share-view-subscriptions.component';
import { ShareViewSubscriptionSnippetsResolver } from './services/resolvers/share-view-subscriptionsnippets.resolver';
import { SharedModule } from '../../shared/shared.module';
import { ShareViewEditComponent } from './modules/edit/share-view-edit.component';
import { ShareViewSettingsComponent } from './modules/settings/share-view-settings.component';
import { ProfileSubscriptionSnippetsResolver } from '../profile/services/resolvers/profile-subscriptionsnippets.resolver';
import { ShareViewCreatePostDialogComponent } from './dialogs/post-create/share-view-create-post-dialog.component';
import { ShareViewSubscriptionTypeDialogComponent } from './dialogs/subscription-type/share-view-subscription-type-dialog.component';
import { ShareViewCreateDashboardSubscriptionDialog } from './dialogs/subscription-create/dashboard/create-dashboard-subscription.dialog';
import { ShareViewCreateWebhookSubscriptionDialog } from './dialogs/subscription-create/webhook/create-webhook-subscription.dialog';
import { ReadmeComponent } from './components/readme/readme.component';
import { MarkdownModule } from 'ngx-markdown';
import { ShareViewCreateDiscordSubscriptionDialog } from './dialogs/subscription-create/discord/create-discord-subscription.dialog';

const routes: Routes = [
  { path: '', redirectTo: 'overview' },
  { path: 'overview', component: ShareViewOverviewComponent },
  {
    path: 'posts', component: ShareViewPostsComponent,
    resolve: { postsResponse: ViewSharePostsResolver }
  },
  {
    path: 'settings', component: ShareViewSettingsComponent,
    loadChildren: () => import("./modules/settings/share-view-settings.module").then(m => m.ShareViewSettingsModule),
    resolve: { secretsResponse: ViewShareSecretsResolver }
  },
  {
    path: 'edit', component: ShareViewEditComponent,
    loadChildren: () => import("./modules/edit/share-view-edit.module").then(m => m.ShareViewEditModule)
  },
  {
    path: 'subscriptions', component: ShareViewSubscriptionsComponent,
    resolve: { subscriptionSnippetsResponse: ShareViewSubscriptionSnippetsResolver }
  }
]

@NgModule({
  declarations: [
    ShareViewComponent,
    ShareViewNavMenuComponent,
    ShareViewOverviewComponent,
    ShareViewPostsComponent,
    ShareViewSubscribeButtonComponent,
    ShareViewSubscriptionsComponent,
    ShareViewSubscriptionTypeDialogComponent,
    ShareViewCreatePostDialogComponent,

    ReadmeComponent,

    ShareViewCreateDashboardSubscriptionDialog,
    ShareViewCreateWebhookSubscriptionDialog,
    ShareViewCreateDiscordSubscriptionDialog,
  ],
  imports: [
    MaterialModule,
    SharedModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    MarkdownModule.forRoot(),
  ],
  providers: [
    ViewSharePostsResolver,
    ViewShareSecretsResolver,
    ShareViewSubscriptionSnippetsResolver,
  ]
})
export class ShareViewModule { }
