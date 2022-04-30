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
