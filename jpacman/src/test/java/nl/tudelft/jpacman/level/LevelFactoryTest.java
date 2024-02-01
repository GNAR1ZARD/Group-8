package nl.tudelft.jpacman.level;

import nl.tudelft.jpacman.npc.ghost.GhostFactory;
import nl.tudelft.jpacman.points.DefaultPointCalculator;
import nl.tudelft.jpacman.sprite.PacManSprites;
import org.junit.jupiter.api.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class LevelFactoryTest {
    private DefaultPointCalculator DPC = new DefaultPointCalculator();

    private static final PacManSprites SPRITE_STORE = new PacManSprites();
    private GhostFactory GstFactory = new GhostFactory(SPRITE_STORE);
    private LevelFactory LvlFactory = new LevelFactory(SPRITE_STORE,GstFactory,DPC);
    @Test
    void testCreatePellet(){
        Pellet testPellet = LvlFactory.createPellet();
        assertThat(testPellet.getValue()).isEqualTo(10);
        assertThat(testPellet.getSprite()).isEqualTo(SPRITE_STORE.getPelletSprite());
    }
}
