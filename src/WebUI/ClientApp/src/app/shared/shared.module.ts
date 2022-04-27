import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/authservice';
import { WeShareClient } from '../../services/weshareclient';
import { ShareSnippetComponent } from './components/share-snippet/share-snippet.component';
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';
import { MarkdownModule } from 'ngx-markdown';
import { ReadmeComponent } from './components/readme/readme.component';
import { MatIconModule } from '@angular/material/icon';
import { LikeButtonComponent } from './components/like-button/like-button.component';
import { MatButtonModule } from '@angular/material/button';
import { NotFoundComponent } from './pages/notfound/notfound.component';
import { ForbiddenComponent } from './pages/forbidden/forbidden.component';
import { SubscriptionSnippetComponent } from './components/subscription-snippet/subscription-snippet.component';

@NgModule({
  declarations: [
    NotFoundComponent,
    ForbiddenComponent,
    ShareSnippetComponent,
    ReadmeComponent,
    LikeButtonComponent,
    SubscriptionSnippetComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([]),

    LoadingBarRouterModule,
    MarkdownModule.forRoot(),

    FormsModule,
    ReactiveFormsModule,

    MatIconModule,
    MatButtonModule,
  ],
  exports: [ 
    NotFoundComponent,
    ForbiddenComponent,

    ReadmeComponent,
    LikeButtonComponent,

    ShareSnippetComponent,
    SubscriptionSnippetComponent,

    CommonModule,

    LoadingBarRouterModule,
    MarkdownModule,

    FormsModule,
    ReactiveFormsModule,

    MatIconModule,
    MatButtonModule,
  ],
  providers: [
    AuthService,
    WeShareClient,
  ]
})
export class SharedModule { }
