const unpm = require('unity-npm-utils');
const path = require('path');

const unitySrcRoot = path.join(__dirname, '..');
const tgtPkgRoot = path.join(unitySrcRoot, '..');

unpm.copyFromUnity2PkgRoot(unitySrcRoot, tgtPkgRoot, {overwrite: true}, (err, info) => {
    if(err) {
        console.log('overwrite-test2src failed with error: %j', err);
        process.exit(1);
        return;
    }

    console.log(`overwrite-test2src succeeded for package ${info.package.name}`);
});
