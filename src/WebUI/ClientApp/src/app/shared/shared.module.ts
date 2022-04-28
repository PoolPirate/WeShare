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
import { PostSnippetComponent } from './components/post-snippet/post-snippet.component';
import { MatCardModule } from '@angular/material/card';
import { MatExpansionModule } from '@angular/material/expansion';

@NgModule({
  declarations: [
    NotFoundComponent,
    ForbiddenComponent,
    ShareSnippetComponent,
    ReadmeComponent,
    LikeButtonComponent,
    SubscriptionSnippetComponent,
    PostSnippetComponent,
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
    MatCardModule,
    MatExpansionModule
  ],
  exports: [ 
    NotFoundComponent,
    ForbiddenComponent,

    ReadmeComponent,
    LikeButtonComponent,

    ShareSnippetComponent,
    SubscriptionSnippetComponent,
    PostSnippetComponent,

    CommonModule,

    LoadingBarRouterModule,
    MarkdownModule,

    FormsModule,
    ReactiveFormsModule,

    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatExpansionModule
  ],
  providers: [
    AuthService,
    WeShareClient,
  ]
})
export class SharedModule { }
