/**
 * The states every screen in this client has to be able to show.
 * `loaded` still covers the empty case — ask the data whether it is empty.
 */
export type LoadState = 'idle' | 'loading' | 'loaded' | 'error';
