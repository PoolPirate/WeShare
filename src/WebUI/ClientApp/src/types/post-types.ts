export class PostSnippet {
  id: number;
  shareId: number;
  createdAt: Date;
  headersSize: number;
  payloadSize: number;
}

export class PostContent {
  headers: Map<string, string[]>;
  payload: Uint8Array;
}

export interface ParsedPostContent {
  headers: object;
  payload: string;
}

export class SentPostInfoDto {
  postSnippet: PostSnippet;

  received: boolean;
  receivedAt: Date;
  attempts: number;
}
