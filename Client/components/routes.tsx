import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './Layout';
import { ZooComponent } from './ZooComponent';

export const routes = <Layout>
    <Route exact path='/'component={ ZooComponent } />
    <Route path='/:slug' render={_ => <div>Not found</div>} />
</Layout>;
